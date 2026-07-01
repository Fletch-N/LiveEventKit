import { Badge, Group, Paper, SimpleGrid, Stack, Text, Title } from '@mantine/core'
import { useAuth } from '../features/auth/useAuth'

const HomePage = () => {
  const { user } = useAuth()

  return (
    <Stack gap="lg">
      <Group justify="space-between" align="flex-start">
        <Stack gap={4}>
          <Title order={1}>Dashboard</Title>
          <Text c="dimmed">Welcome back, {user?.name}.</Text>
        </Stack>
        <Badge variant="light">Authenticated</Badge>
      </Group>

      <SimpleGrid cols={{ base: 1, sm: 3 }}>
        <Paper p="md">
          <Text fw={700} size="xl">
            0
          </Text>
          <Text c="dimmed">Active events</Text>
        </Paper>
        <Paper p="md">
          <Text fw={700} size="xl">
            0
          </Text>
          <Text c="dimmed">Checked-in attendees</Text>
        </Paper>
        <Paper p="md">
          <Text fw={700} size="xl">
            0
          </Text>
          <Text c="dimmed">Open tasks</Text>
        </Paper>
      </SimpleGrid>
    </Stack>
  )
}

export default HomePage
