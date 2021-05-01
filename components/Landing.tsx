import styles from './Landing.module.scss'
import classNames from 'classnames'
import type { ReactElement, ReactNode } from 'react'
import { Button } from 'react-bootstrap'
import { BsGeoAlt } from 'react-icons/bs'
import { FaRegThumbsDown, FaRegThumbsUp } from 'react-icons/fa'

function Feature (props: { heading: ReactNode, children: ReactNode, icon: ReactNode, isIconLeft: boolean }): ReactElement {
  return (
    <section className='py-4'>
      <div className='row align-items-center'>
        <div className='col-12 col-lg-10'>
          <h1 className='display-5'>{props.heading}</h1>
          <div className='pt-4'>{props.children}</div>
        </div>
        <div className={classNames('col-12', 'col-lg-2', { 'order-lg-first': props.isIconLeft }, styles.iconcol)}>
          {props.icon}
        </div>
      </div>
    </section>
  )
}

function ThumbsIcon (): ReactElement {
  return (
    <svg stroke='currentColor' fill='currentColor' strokeWidth='0' viewBox='0 0 2 1' width='100%'>
      <g>
        <FaRegThumbsUp size='1' />
      </g>
      <g transform='translate(1, 0)'>
        <FaRegThumbsDown size='1' />
      </g>
    </svg>
  )
}

export default function Landing (): ReactElement {
  return (
    <main>
      <div className={styles.banner} role='banner'>
        <div className={styles.bannerEyecatch} />
        <div className={`${styles.bannerContent} text-light`}>
          <section className='container'>
            <h1 className='display-4' style={{ whiteSpace: 'nowrap' }}>
              まとめて、<wbr />選んで、<wbr />運まかせ。
            </h1>
            <p className='lead'>
              あなたの行きつけコレクションから、納得の行き先を選びましょう。
            </p>
          </section>
        </div>
      </div>

      <div className='container'>
        <Feature
          heading='チェックインからはじめよう'
          icon={<BsGeoAlt size='100%' />}
          isIconLeft
        >
          <p className='lead'>もしかして、あのお店のメイヤーですか？ Swarmのチェックイン履歴から、おすすめのお店をまとめましょう。</p>
        </Feature>
        <hr />
        <Feature
          heading='今日の気分をぶつけよう'
          icon={<ThumbsIcon />}
          isIconLeft={false}
        >
          <p className='lead'>
            最近、麺類が続いて飽きましたか？ 投票機能であなたの気分を反映することができます。
          </p>
          <p className='lead'>
            グループでも大丈夫。QRコードを共有して、みんなで投票しましょう。
          </p>
        </Feature>
        <hr />
        <section className='py-4 text-center'>
          <h1 className='display-5'>
            運任せで、もう迷わない
          </h1>
          <p className='lead pt-4'>
            最後はルーレットにすべてお任せ。優柔不断でも、意見が割れても、運がなんとかしてくれます。
          </p>
        </section>
        <section className='pb-4 d-grid gap-3 col-12 col-md-8 col-lg-6 col-xl-5 mx-auto'>
          <Button variant='svc-foursquare'>Foursquareではじめる</Button>
          <Button variant='svc-twitter'>Twitterではじめる</Button>
        </section>
      </div>
    </main>
  )
}
