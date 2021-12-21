import http from 'k6/http';
import { sleep } from 'k6';

export let options = {
    insecureSkipTLSVerify: true,
    noConnectionReuse: false,
    stages: [
        { duration: '2m', target: 100 }, // below normal load
        { duration: '5m', target: 100 },
        { duration: '2m', target: 300 }, // normal load
        { duration: '5m', target: 300 },
        { duration: '2m', target: 500 },
        { duration: '5m', target: 500 },
        { duration: '2m', target: 700 },
        { duration: '5m', target: 700 },
        { duration: '10m', target: 0 } // scale down. Recovery stage
    ]    
};

const API_BASE_URL = 'https://localhost:7194/api';

export default () => {
    http.get(API_BASE_URL + '/Users');
}