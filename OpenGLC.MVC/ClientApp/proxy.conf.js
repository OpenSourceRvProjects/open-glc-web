const { env } = require('process');

const target = env.ASPNETCORE_HTTPS_PORT ? `https://localhost:${env.ASPNETCORE_HTTPS_PORT}` :
  env.ASPNETCORE_URLS ? env.ASPNETCORE_URLS.split(';')[0] : 'http://localhost:62575';

const PROXY_CONFIG = [
  {
    context: [
      "/weatherforecast",
      "/api/Account/register",
      "/api/Account/login",
      "/api/MealEvents/userEventMetrics",
      "/api/MealEvents/getEventMealTypes",
      "/api/MealEvents",
      "/api/MealItems",
      "/api/MealEvents"

   ],
    proxyTimeout: 10000,
    target: target,
    secure: false,
    headers: {
      Connection: 'Keep-Alive'
    }
  }
]

module.exports = PROXY_CONFIG;
