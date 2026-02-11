import { FetchRequestAdapter } from '@microsoft/kiota-http-fetchlibrary';
import { createApiClient } from '@/api';

const authProvider = {
  authenticateRequest() {
    return Promise.resolve();
  },
};

const adapter = new FetchRequestAdapter(authProvider);
const client = createApiClient(adapter);

export default () => {
  return client.api;
};
