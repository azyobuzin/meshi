import Link, { LinkProps } from 'next/link'
import type { AnchorHTMLAttributes, PropsWithChildren, ReactElement, ReactNode } from 'react'
import { Container, Navbar } from 'react-bootstrap'

function LinkWithProps (props: PropsWithChildren<AnchorHTMLAttributes<HTMLAnchorElement> & LinkProps>): ReactElement {
  const { href, replace, scroll, shallow, passHref, prefetch, locale, ...aProps } = props
  const linkProps: LinkProps = { href, replace, scroll, shallow, passHref, prefetch, locale }
  return <Link {...linkProps}><a {...aProps} /></Link>
}

function Header (): ReactElement {
  return (
    <header>
      <Navbar sticky='top' variant='light' bg='light'>
        <Container>
          <Navbar.Brand
            as={LinkWithProps}
            href='/'
            role='banner'
          >
            <a>昼飯ルーレット</a>
          </Navbar.Brand>
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
