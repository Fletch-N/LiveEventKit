import type { ReactNode } from 'react'
import { useMemo, useState } from 'react'
import { AuthContext, type AuthContextValue } from './AuthContext'
import {
  clearStoredSession,
  readStoredSession,
  saveStoredSession,
} from './authStorage'
import type { AuthSession, LoginCredentials } from './types'

type AuthProviderProps = {
  children: ReactNode
}

const getDisplayName = (email: string) => {
  const [name] = email.split('@')
  return name || 'Event Manager'
}

export const AuthProvider = ({ children }: AuthProviderProps) => {
  const [session, setSession] = useState<AuthSession | null>(() =>
    readStoredSession(),
  )

  const value = useMemo<AuthContextValue>(() => {
    const login = ({ email, remember }: LoginCredentials) => {
      const nextSession: AuthSession = {
        accessToken: `local-session-${Date.now()}`,
        user: {
          email,
          name: getDisplayName(email),
        },
      }

      setSession(nextSession)

      if (remember) {
        saveStoredSession(nextSession)
      } else {
        clearStoredSession()
      }
    }

    const logout = () => {
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
