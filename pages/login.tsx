import Head from 'next/head'
import { useRouter } from 'next/router'
import { ReactElement, useEffect } from 'react'
import { Button } from 'react-bootstrap'
import Layout from '../components/Layout'
import { signInWithTwitter, useUser } from '../utils/auth'
import niceCatch from '../utils/nice-catch'

export default function Login (): ReactElement {
  const router = useRouter()
  const { user } = useUser()

  function redirect (): Promise<boolean> {
    let returnTo = router.query.returnTo
    if (typeof returnTo !== 'string' || returnTo === '') { returnTo = '/my' }
    return router.push(returnTo)
  }

  // Redirect if logged in or when logging in succeeds
  useEffect(() => {
    if (user != null) niceCatch(redirect())
  }, [user])

  function handleLoginWithTwitter (): void {
    niceCatch(signInWithTwitter())
  }

  return (
    <Layout>
      <Head>
        <title>ログイン / 昼飯ルーレット</title>
      </Head>

      <main className='my-5 container-xxl'>
        <div className='mx-auto' style={{ maxWidth: '350px' }}>
          <h1 className='h3 text-center mb-4'>ログイン</h1>
          <div className='d-grid gap-2'>
            <Button variant='svc-foursquare'>Foursquare</Button>
            <Button variant='svc-twitter' onClick={handleLoginWithTwitter}>Twitter</Button>
          </div>
        </div>
      </main>
    </Layout>
  )
}
