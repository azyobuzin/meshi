import Link from 'next/link'
import type { ReactElement } from 'react'
import { Card } from 'react-bootstrap'
import { BsGeoAlt, BsPerson } from 'react-icons/bs'

export interface CollectionSummary {
  readonly id: string
  readonly name: string
  readonly description: string
  readonly creatorName: string
  readonly numberOfPlaces: number
}

export interface CollectionsProps {
  collections: readonly CollectionSummary[]
  showCreator: boolean
}

function Collection ({ collection, showCreator }: { collection: CollectionSummary, showCreator: boolean }): ReactElement {
  const href = {
    pathname: '/collection/[id]',
    query: { id: collection.id }
  }

  return (
    <div className='col' role='listitem'>
      <Card>
        <Card.Body>
          <Card.Title>
            <Link href={href} prefetch={false}>
              <a className='text-reset text-decoration-none stretched-link'>{collection.name}</a>
            </Link>
          </Card.Title>
          {showCreator &&
            <Card.Subtitle className='mb-2 text-muted fw-normal'>
              <BsPerson />
              {' ' + collection.creatorName}
            </Card.Subtitle>}
          <Card.Text style={{ whiteSpace: 'pre-wrap' }}>
            {collection.description}
          </Card.Text>
        </Card.Body>
        <Card.Footer className='text-muted'>
          <BsGeoAlt />
          {` ${collection.numberOfPlaces} 地点`}
        </Card.Footer>
      </Card>
    </div>
  )
}

export function Collections ({ collections, showCreator }: CollectionsProps): ReactElement {
  return (
    <div className='row row-cols-1 row-cols-md-2 row-cols-xl-3 g-2' role='list'>
      {collections.map(x => <Collection key={x.id} collection={x} showCreator={showCreator} />)}
    </div>
  )
}
