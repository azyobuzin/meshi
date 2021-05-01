import Head from 'next/head'
import type { ReactElement } from 'react'
import Landing from '../components/Landing'
import Layout from '../components/Layout'

export default function Home (): ReactElement {
  return (
    <Layout>
      <Head>
        <title>昼飯ルーレット</title>
      </Head>

      <Landing />
    </Layout>
  )
}
