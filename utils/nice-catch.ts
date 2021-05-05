export default function niceCatch (promise: Promise<unknown>): void {
  promise.catch(err => console.error(err))
}
