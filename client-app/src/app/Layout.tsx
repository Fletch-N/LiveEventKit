import {
  AppShell,
  Burger,
  Button,
  Group,
  NavLink,
  Stack,
  Text,
  Title,
} from '@mantine/core'
import { useDisclosure } from '@mantine/hooks'
import { NavLink as RouterNavLink, Outlet, useNavigate } from 'react-router'
import { useAuth } from '../features/auth/useAuth'

const Layout = () => {
  const [opened, { toggle }] = useDisclosure()
  const { logout, user } = useAuth()
  const navigate = useNavigate()

  const handleLogout = () => {
    logout()
    navigate('/login', { replace: true })
  }

  return (
    <AppShell
      header={{ height: 60 }}
      navbar={{
        width: 260,
        breakpoint: 'sm',
        collapsed: { mobile: !opened },
      }}
      padding="md"
    >
      <AppShell.Header>
        <Group h="100%" px="md" justify="space-between">
          <Group gap="sm">
            <Burger opened={opened} onClick={toggle} hiddenFrom="sm" size="sm" />
            <Title order={2} size="h4">
              LiveEventKit
            </Title>
          </Group>
          <Button variant="subtle" onClick={handleLogout}>
            Sign out
          </Button>
        </Group>
      </AppShell.Header>

      <AppShell.Navbar p="md">
        <Stack gap="lg" h="100%" justify="space-between">
          <Stack gap="xs">
            <Text size="xs" c="dimmed" tt="uppercase" fw={700}>
              Workspace
            </Text>
            <NavLink component={RouterNavLink} to="/" label="Dashboard" />
          </Stack>

          <Stack gap={2}>
            <Text fw={600}>{user?.name}</Text>
            <Text c="dimmed">{user?.email}</Text>
          </Stack>
        </Stack>
      </AppShell.Navbar>

      <AppShell.Main>
        <Outlet />
      </AppShell.Main>
    </AppShell>
  )
}

export default Layout
