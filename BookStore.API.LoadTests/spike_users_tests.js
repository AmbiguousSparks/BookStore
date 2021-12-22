import http from 'k6/http';
import {sleep} from 'k6';

export let options = {
    insecureSkipTLSVerify: true,
    noConnectionsReuse: false,
    stages: [
        {duration: '5m', target: 200},
        {duration: '3m', target: 1500},
        {duration: '5m', target: 200}
    ]
}

const API_BASE_URL = "https://localhost:7194/api"

export default () => {
    http.batch([
        ['GET', `${API_BASE_URL}/Users`]
    ])
    sleep(1)
}