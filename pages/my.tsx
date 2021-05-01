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

export default function MyCollections (): ReactElement {
  return (
    <Layout>
      <Head>
        <title>あなたのコレクション / 昼飯ルーレット</title>
      </Head>

      {/* TODO: お気に入り */}

      <section className='py-4rem container'>
        <h1 className='h3 mb-4'>あなたのコレクション</h1>
        <Collections collections={collections} showCreator={false} />
      </section>
    </Layout>
  )
}
