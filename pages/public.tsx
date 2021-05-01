import Head from 'next/head'
import type { ReactElement } from 'react'
import { Collections } from '../components/Collections'
import Layout from '../components/Layout'

const collections = [
  {
    id: '1',
    name: 'hoge',
    description: 'a\nb\nc',
    creatorName: 'azyobuzin',
    numberOfPlaces: 3
  },
  {
    id: '2',
    name: 'hoge',
    description: 'a\nb\nc',
    creatorName: 'azyobuzin',
    numberOfPlaces: 3
  },
  {
    id: '3',
    name: 'hoge',
    description: 'a\nb\nc',
    creatorName: 'azyobuzin',
    numberOfPlaces: 3
  },
  {
    id: '4',
    name: 'hoge',
    description: 'a\nb\nc',
    creatorName: 'azyobuzin',
    numberOfPlaces: 3
  },
  {
    id: '5',
    name: 'hoge',
    description: 'a\nb\nc',
    creatorName: 'azyobuzin',
    numberOfPlaces: 3
  }
]

export default function PublicCollections (): ReactElement {
  return (
    <Layout>
      <Head>
        <title>公開コレクション / 昼飯ルーレット</title>
      </Head>

      <main className='py-4rem container'>
        <h1 className='h3 mb-4'>公開コレクション</h1>
        <Collections collections={collections} showCreator />
      </main>
    </Layout>
  )
}
