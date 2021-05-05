import { FirebaseApp, initializeApp as initializeFirebase } from 'firebase/app'
import { TwitterAuthProvider, User, signOut as firebaseSignOut, getAuth, onAuthStateChanged, signInWithPopup } from 'firebase/auth'
import { useEffect, useState } from 'react'

let firebaseApp: FirebaseApp | undefined

if (process.browser) {
  const firebaseConfig = {
    apiKey: process.env.NEXT_PUBLIC_FIREBASE_API_KEY,
    authDomain: process.env.NEXT_PUBLIC_FIREBASE_AUTH_DOMAIN
  }
  if (firebaseConfig.apiKey != null && firebaseConfig.authDomain != null) {
    firebaseApp = initializeFirebase(firebaseConfig)
  } else {
    console.warn('Firebase is not initialized')
  }
}

export interface UseUserState {
  readonly isLoading: boolean
  readonly user: User | null
}

export function useUser (): UseUserState {
  const firebaseEnabled = firebaseApp != null
  const [state, setState] = useState<UseUserState>({
    isLoading: firebaseEnabled,
    user: firebaseEnabled ? getAuth(firebaseApp).currentUser : null
  })

  useEffect(
    () => {
      if (!firebaseEnabled) return
      return onAuthStateChanged(
        getAuth(firebaseApp),
        user => setState({ isLoading: false, user })
      )
    },
    []
  )

  return state
}

export async function signOut (): Promise<void> {
  if (firebaseApp == null) return

  const auth = getAuth(firebaseApp)
  await firebaseSignOut(auth)
}

export async function signInWithTwitter (): Promise<void> {
  if (firebaseApp == null) return

  const auth = getAuth(firebaseApp)
  const provider = new TwitterAuthProvider()

  try {
    await signInWithPopup(auth, provider)
  } catch (err) {
    // ポップアップ系はユーザーの操作で起こりえる
    if ((err.code as string | undefined)?.includes('popup') === false) {
      throw err
    }
  }
}
