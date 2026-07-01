import { type SubmitEvent, useState } from 'react'
import {
  Button,
  Center,
  Checkbox,
  Paper,
  PasswordInput,
  Stack,
  Text,
  TextInput,
  Title,
} from '@mantine/core'
import { Navigate, useLocation, useNavigate } from 'react-router'
import { useAuth } from '../features/auth/useAuth'

type RedirectState = {
  from?: {
    pathname?: string
    search?: string
  }
}

const LoginPage = () => {
  const { isAuthenticated, login } = useAuth()
  const navigate = useNavigate()
  const location = useLocation()
  const [email, setEmail] = useState('')
  const [password, setPassword] = useState('')
  const [remember, setRemember] = useState(true)
  const [formError, setFormError] = useState<string | null>(null)

  const redirectState = location.state as RedirectState | null
  const redirectPath = redirectState?.from?.pathname ?? '/'
  const redirectSearch = redirectState?.from?.search ?? ''
  const redirectTo =
    redirectPath === '/login' ? '/' : `${redirectPath}${redirectSearch}`

  if (isAuthenticated) {
    return <Navigate to={redirectTo} replace />
  }

  const handleSubmit = (event: SubmitEvent<HTMLFormElement>) => {
    event.preventDefault()

    if (!email.trim() || !password) {
      setFormError('Email and password are required.')
      return
    }

    login({ email: email.trim(), password, remember })
    navigate(redirectTo, { replace: true })
  }

  return (
    <Center mih="100svh" px="md" bg="gray.0">
      <Paper w="100%" maw={420} p="xl">
        <Stack gap="lg">
          <Stack gap={4}>
            <Title order={1}>Sign in</Title>
            <Text c="dimmed">Access your live event workspace.</Text>
          </Stack>

          <form onSubmit={handleSubmit}>
            <Stack>
              <TextInput
                autoComplete="email"
                error={formError}
                label="Email"
                onChange={(event) => {
                  setEmail(event.currentTarget.value)
                  setFormError(null)
                }}
                placeholder="manager@example.com"
                type="email"
                value={email}
                required
              />
              <PasswordInput
                autoComplete="current-password"
                error={formError}
                label="Password"
                onChange={(event) => {
                  setPassword(event.currentTarget.value)
                  setFormError(null)
                }}
                placeholder="Your password"
                value={password}
                required
              />
              <Checkbox
                checked={remember}
                label="Keep me signed in"
                onChange={(event) =>
                  setRemember(event.currentTarget.checked)
                }
              />
              <Button type="submit" fullWidth>
                Sign in
              </Button>
            </Stack>
          </form>
        </Stack>
      </Paper>
    </Center>
  )
}

export default LoginPage
