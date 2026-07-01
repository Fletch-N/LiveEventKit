import { Button, Center, Stack, Text, Title } from '@mantine/core'
import { Link } from 'react-router'

const NotFoundPage = () => {
  return (
    <Center mih="100svh" px="md">
      <Stack align="center" gap="md">
        <Title order={1}>Page not found</Title>
        <Text c="dimmed">The page you requested does not exist.</Text>
        <Button component={Link} to="/">
          Go to dashboard
        </Button>
      </Stack>
    </Center>
  )
}

export default NotFoundPage
