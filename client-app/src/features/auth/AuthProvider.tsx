import type { ReactNode } from 'react'
import { useMemo, useState } from 'react'
import { AuthContext, type AuthContextValue } from './AuthContext'
import { authApi } from './api'
import {
  clearStoredSession,
  readStoredSession,
  saveStoredSession,
} from './authStorage'
import type { AuthSession, LoginCredentials } from './types'

type AuthProviderProps = {
  children: ReactNode
}

export const AuthProvider = ({ children }: AuthProviderProps) => {
  const [session, setSession] = useState<AuthSession | null>(() =>
    readStoredSession(),
  )

  const value = useMemo<AuthContextValue>(() => {
    const login = async (credentials: LoginCredentials) => {
      const nextSession = await authApi.login(credentials)
      setSession(nextSession)
      saveStoredSession(nextSession, credentials.remember)
    }

    const logout = async () => {
      await authApi.logout().catch(() => undefined)
      setSession(null)
      clearStoredSession()
    }

    return {
      isAuthenticated: !!session,
      login,
      logout,
      session,
      user: session?.user ?? null,
    }
  }, [session])

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>
}
