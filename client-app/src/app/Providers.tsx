import type { ReactNode } from 'react'
import { MantineProvider } from '@mantine/core'
import { AuthProvider } from '../features/auth/AuthProvider'
import { theme } from './Theme'

type ProvidersProps = {
  children: ReactNode
}

const Providers = ({ children }: ProvidersProps) => {
  return (
    <MantineProvider theme={theme}>
      <AuthProvider>{children}</AuthProvider>
    </MantineProvider>
  )
}

export default Providers
