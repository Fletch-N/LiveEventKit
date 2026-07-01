import {
  Button,
  Card,
  Container,
  Input,
  Modal,
  Paper,
  Text,
  Title,
  createTheme,
  rem,
} from '@mantine/core'

export const theme = createTheme({
  primaryColor: 'brand',
  primaryShade: { light: 6, dark: 4 },
  defaultRadius: 'md',
  fontFamily:
    'Inter, ui-sans-serif, system-ui, -apple-system, BlinkMacSystemFont, "Segoe UI", sans-serif',
  fontFamilyMonospace:
    '"JetBrains Mono", "SFMono-Regular", Consolas, "Liberation Mono", monospace',

  colors: {
    brand: [
      '#eff6ff',
      '#dbeafe',
      '#bfdbfe',
      '#93c5fd',
      '#60a5fa',
      '#3b82f6',
      '#2563eb',
      '#1d4ed8',
      '#1e40af',
      '#1e3a8a',
    ],
  },

  headings: {
    fontFamily:
      'Inter, ui-sans-serif, system-ui, -apple-system, BlinkMacSystemFont, "Segoe UI", sans-serif',
    fontWeight: '700',
    sizes: {
      h1: { fontSize: rem(40), lineHeight: '1.1' },
      h2: { fontSize: rem(32), lineHeight: '1.15' },
      h3: { fontSize: rem(24), lineHeight: '1.2' },
      h4: { fontSize: rem(20), lineHeight: '1.25' },
      h5: { fontSize: rem(18), lineHeight: '1.3' },
      h6: { fontSize: rem(16), lineHeight: '1.35' },
    },
  },

  spacing: {
    xs: rem(8),
    sm: rem(12),
    md: rem(16),
    lg: rem(24),
    xl: rem(32),
  },

  radius: {
    xs: rem(4),
    sm: rem(6),
    md: rem(8),
    lg: rem(12),
    xl: rem(16),
  },

  shadows: {
    xs: '0 1px 2px rgba(15, 23, 42, 0.06)',
    sm: '0 1px 3px rgba(15, 23, 42, 0.1), 0 1px 2px rgba(15, 23, 42, 0.06)',
    md: '0 8px 24px rgba(15, 23, 42, 0.1)',
    lg: '0 16px 40px rgba(15, 23, 42, 0.12)',
    xl: '0 24px 64px rgba(15, 23, 42, 0.16)',
  },

  components: {
    Button: Button.extend({
      defaultProps: {
        radius: 'md',
        size: 'sm',
      },
    }),
    Card: Card.extend({
      defaultProps: {
        radius: 'md',
        shadow: 'sm',
        withBorder: true,
      },
    }),
    Container: Container.extend({
      defaultProps: {
        size: 'lg',
      },
    }),
    Input: Input.extend({
      defaultProps: {
        radius: 'md',
      },
    }),
    Modal: Modal.extend({
      defaultProps: {
        centered: true,
        radius: 'md',
        overlayProps: {
          backgroundOpacity: 0.45,
          blur: 3,
        },
      },
    }),
    Paper: Paper.extend({
      defaultProps: {
        radius: 'md',
        shadow: 'xs',
        withBorder: true,
      },
    }),
    Text: Text.extend({
      defaultProps: {
        size: 'sm',
      },
    }),
    Title: Title.extend({
      defaultProps: {
        textWrap: 'balance',
      },
    }),
  },
})
