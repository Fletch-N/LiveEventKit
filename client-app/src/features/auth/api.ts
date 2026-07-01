import { apiRequest, requests } from '../../lib/agent'
import type { AuthSession, AuthUser, LoginCredentials } from './types'

type LoginResponse = {
  accessToken?: string
  token?: string
}

type BackendUser = {
  displayName?: string
  email?: string
  name?: string
  userName?: string
}

const getAccessToken = (response: LoginResponse) =>
  response.accessToken ?? response.token

const toAuthUser = (user: BackendUser, fallbackEmail: string): AuthUser => {
  const email = user.email ?? user.userName ?? fallbackEmail
  const name = user.name ?? user.displayName ?? email.split('@')[0] ?? email

  return { email, name }
}

export const authApi = {
  login: async ({
    email,
    password,
  }: LoginCredentials): Promise<AuthSession> => {
    const response = await requests.post<LoginResponse>(
      '/auth/login?useCookies=false',
      {
        email,
        password,
      },
    )
    const accessToken = getAccessToken(response)

    if (!accessToken) {
      throw new Error('Login response did not include an access token.')
    }

    const user = await apiRequest<BackendUser>('/auth/me', {
      headers: {
        Authorization: `Bearer ${accessToken}`,
      },
    })

    return {
      accessToken,
      user: toAuthUser(user, email),
    }
  },

  logout: () => requests.post<unknown>('/auth/logout', {}),
}
