export type AuthUser = {
  email: string
  name: string
}

export type AuthSession = {
  accessToken: string
  user: AuthUser
}

export type LoginCredentials = {
  email: string
  password: string
  remember: boolean
}
