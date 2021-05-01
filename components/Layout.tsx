import Link from 'next/link'
import type { ReactElement, ReactNode } from 'react'
import { Container, Navbar } from 'react-bootstrap'

function Header (): ReactElement {
  return (
    <header>
      <Navbar sticky='top' variant='light' bg='light'>
        <Container fluid='xxl'>
          <Link href='/' passHref prefetch={false}>
            <Navbar.Brand>昼飯ルーレット</Navbar.Brand>
          </Link>
        </Container>
      </Navbar>
    </header>
  )
}

export default function Layout ({ children }: {children?: ReactNode}): ReactElement {
  return (
    <>
      <Header />
      {children}
    </>
  )
}
