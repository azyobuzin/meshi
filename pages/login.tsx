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

      <main className='py-4rem container'>
        <div className='row align-items-start'>
          <div className='col-md col-xl-7 pb-4'>
            <h1 className='display-6 text-center'>ログイン</h1>
          </div>
          <div className='col-md col-xl-5 d-grid gap-2'>
            <Button variant='svc-foursquare'>Foursquare</Button>
            <Button variant='svc-twitter'>Twitter</Button>
          </div>
        </div>
      </main>
    </Layout>
  )
}
