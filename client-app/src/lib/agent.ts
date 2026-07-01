import { clearStoredSession, readStoredSession } from '../features/auth/authStorage'

const API_BASE_URL = import.meta.env.VITE_API_BASE_URL

type HttpMethod = 'DELETE' | 'GET' | 'POST' | 'PUT'

type ApiRequestOptions = Omit<RequestInit, 'body' | 'method'> & {
  body?: unknown
  method?: HttpMethod
}

export class ApiError extends Error {
  data: unknown
  response: Response
  status: number

  constructor(response: Response, data: unknown) {
    super(`API request failed: ${response.status}`)
    this.name = 'ApiError'
    this.data = data
    this.response = response
    this.status = response.status
  }
}

const isFormData = (body: unknown): body is FormData => body instanceof FormData

const isExpiredBearerToken = (response: Response) => {
  const authHeader = response.headers.get('www-authenticate')

  return (
    response.status === 401 &&
    !!authHeader &&
    authHeader.includes('Bearer') &&
    authHeader.includes('invalid_token') &&
    authHeader.includes('The token expired')
  )
}

const getResponseBody = async (response: Response) => {
  if (response.status === 204) {
    return null
  }

  const contentType = response.headers.get('content-type')

  if (contentType?.includes('application/json')) {
    return response.json()
  }

  return response.text()
}

const handleErrorResponse = async (response: Response, method: HttpMethod) => {
  const data = await getResponseBody(response)

  if (response.status === 404) {
    window.location.assign('/notfound')
  }

  if (isExpiredBearerToken(response)) {
    clearStoredSession()
    window.location.assign('/login')
  }

  if (
    response.status === 400 &&
    method === 'GET' &&
    typeof data === 'object' &&
    data !== null &&
    'errors' in data &&
    typeof data.errors === 'object' &&
    data.errors !== null &&
    'id' in data.errors
  ) {
    window.location.assign('/notfound')
  }

  throw new ApiError(response, data)
}

export async function apiRequest<T>(
  path: string,
  { body, headers, method = 'GET', ...options }: ApiRequestOptions = {},
): Promise<T> {
  const session = readStoredSession()
  const requestHeaders = new Headers(headers)

  if (session?.accessToken) {
    requestHeaders.set('Authorization', `Bearer ${session.accessToken}`)
  }

  let requestBody: BodyInit | undefined

  if (isFormData(body)) {
    requestBody = body
  } else if (body !== undefined) {
    requestHeaders.set('Content-Type', 'application/json')
    requestBody = JSON.stringify(body)
  }

  try {
    const response = await fetch(`${API_BASE_URL}${path}`, {
      ...options,
      body: requestBody,
      headers: requestHeaders,
      method,
    })

    if (!response.ok) {
      return handleErrorResponse(response, method)
    }

    return getResponseBody(response) as Promise<T>
  } catch (error) {
    if (error instanceof ApiError) {
      throw error
    }

    throw new Error('Network error - API is probably down', { cause: error })
  }
}

export const requests = {
  del: <T>(url: string) => apiRequest<T>(url, { method: 'DELETE' }),
  get: <T>(url: string) => apiRequest<T>(url),
  post: <T>(url: string, body: unknown) =>
    apiRequest<T>(url, { body, method: 'POST' }),
  postForm: <T>(url: string, file: Blob) => {
    const formData = new FormData()
    formData.append('File', file)

    return apiRequest<T>(url, { body: formData, method: 'POST' })
  },
  put: <T>(url: string, body: unknown) =>
    apiRequest<T>(url, { body, method: 'PUT' }),
}
