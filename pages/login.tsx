import Head from 'next/head'
import type { ReactElement } from 'react'
import { Button } from 'react-bootstrap'
import Layout from '../components/Layout'

export default function Login (): ReactElement {
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
            <Button variant='svc-twitter'>Twitter</Button>
          </div>
        </div>
      </main>
    </Layout>
  )
}
