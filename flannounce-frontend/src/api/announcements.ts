import { BACKEND_URL } from '@/constants';
import { Announcement } from '@/types/announcement';

interface Endpoint {
  path: string;
  method?: 'GET' | 'POST' | 'DELETE' | 'PUT';
  data?: object;
}

const fetchEndpoint = async ({ path, method, data }: Endpoint) => {
  const url = BACKEND_URL + path;

  const config: RequestInit = {
    method,
    headers: {
      Accept: 'application/json',
      'Content-Type': 'application/json'
    },
    body: JSON.stringify(data)
  };

  const response = await window.fetch(url, config);

  try {
    return await response.json();
  } catch (exception) {
    throw new Error('Could not parse JSON');
  }
};

function fetchAnnouncements(): Promise<Announcement[]> {
  return fetchEndpoint({ path: 'announce' }).then(response => response.data);
}

// function addFlat(flat: IFlat): Promise<IFlat> {
//   return fetchEndpoint({ path: 'flat', method: 'POST', data: flat })
// }

// function updateFlat(flat: IFlat): Promise<IFlat> {
//   return fetchEndpoint({ path: `flat/${flat.flatId}`, method: 'PUT', data: flat })
// }

// function deleteFlat(id: string): Promise<void> {
//   return fetchEndpoint({ path: `flat/${id}`, method: 'DELETE' })
// }

export default {
  fetchAnnouncements
};
