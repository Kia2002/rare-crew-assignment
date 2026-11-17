// Used only by `ng serve`. The key is added here, on the dev server,
// so it never ends up in the JavaScript that is sent to the browser.
const apiKey = process.env.TIME_ENTRY_API_KEY;

if (!apiKey) {
  throw new Error(
    'API key is missing. Set it in your terminal with: $env:TIME_ENTRY_API_KEY = "<your-key>"',
  );
}

module.exports = {
  '/api': {
    target: 'https://rc-vault-fap-live-1.azurewebsites.net',
    changeOrigin: true,
    rewrite: (path) => `${path}?code=${encodeURIComponent(apiKey)}`,
  },
};
