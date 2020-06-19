interface Endpoint {
  path: string
  method?: 'GET' | 'POST' | 'DELETE' | 'PUT'
  data?: object
}

export const fetchEndpoint = async ({ path, method, data }: Endpoint) => {
  const url = process.env.VUE_APP_API_BASE_URL + path

  const config: RequestInit = {
    method,
    headers: {
      Accept: 'application/json',
      'Content-Type': 'application/json'
    },
    body: JSON.stringify(data)
  }

  const response = await window.fetch(url, config)

  try {
    return await response.json()
  } catch (exception) {
    throw new Error('Could not parse JSON')
  }
}

export function objectToParams<T extends object>(obj: T) {
  return (Object.keys(obj) as Array<keyof T>)
    .map(key => key + '=' + obj[key])
    .join('&')
}
