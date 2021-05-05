import type { User } from 'firebase/auth'
import Link from 'next/link'
import type { ReactElement, ReactNode } from 'react'
import { Container, Nav, NavDropdown, Navbar } from 'react-bootstrap'
import { signOut, useUser } from '../utils/auth'
import niceCatch from '../utils/nice-catch'

function GuestNav (): ReactElement {
  return (
    <Nav>
      <Link href='/login' passHref prefetch={false}>
        <Nav.Link>ログイン</Nav.Link>
      </Link>
    </Nav>
  )
}

function LoggedInNav ({ user }: {user: User}): ReactElement {
  function handleLogout (): void {
    niceCatch(signOut())
  }

  return (
    <Nav>
      <NavDropdown title={user.displayName} id='header-user-dropdown'>
        <Link href='/my' passHref prefetch={false}>
          <NavDropdown.Item>コレクション</NavDropdown.Item>
        </Link>
        <NavDropdown.Divider />
        <NavDropdown.Item onClick={handleLogout}>ログアウト</NavDropdown.Item>
      </NavDropdown>
    </Nav>
  )
}

function Header (): ReactElement {
  const { isLoading, user } = useUser()

  return (
    <header>
      <Navbar sticky='top' variant='light' bg='light'>
        <Container fluid='xxl'>
          <Link href='/' passHref prefetch={false}>
            <Navbar.Brand>昼飯ルーレット</Navbar.Brand>
          </Link>
          {!isLoading && (user != null ? <LoggedInNav user={user} /> : <GuestNav />)}
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
