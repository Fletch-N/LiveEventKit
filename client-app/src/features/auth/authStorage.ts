import type { AuthSession } from './types'

const AUTH_SESSION_STORAGE_KEY = 'live-event-kit.auth-session'

const isAuthSession = (value: unknown): value is AuthSession => {
  if (!value || typeof value !== 'object') {
    return false
  }

  const session = value as Partial<AuthSession>

  return (
    typeof session.accessToken === 'string' &&
    !!session.user &&
    typeof session.user.email === 'string' &&
    typeof session.user.name === 'string'
  )
}

export const readStoredSession = () => {
  const storedSession = window.localStorage.getItem(AUTH_SESSION_STORAGE_KEY)

  if (!storedSession) {
    return null
  }

  try {
    const parsedSession: unknown = JSON.parse(storedSession)
    return isAuthSession(parsedSession) ? parsedSession : null
  } catch {
    return null
  }
}

export const saveStoredSession = (session: AuthSession) => {
  window.localStorage.setItem(AUTH_SESSION_STORAGE_KEY, JSON.stringify(session))
}

export const clearStoredSession = () => {
  window.localStorage.removeItem(AUTH_SESSION_STORAGE_KEY)
}
