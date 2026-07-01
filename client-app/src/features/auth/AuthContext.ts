import { createContext } from 'react'
import type { AuthSession, AuthUser, LoginCredentials } from './types'

export type AuthContextValue = {
  isAuthenticated: boolean
  login: (credentials: LoginCredentials) => Promise<void>
  logout: () => Promise<void>
  session: AuthSession | null
  user: AuthUser | null
}

export const AuthContext = createContext<AuthContextValue | null>(null)
