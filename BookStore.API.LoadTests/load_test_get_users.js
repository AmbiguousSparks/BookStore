import http from 'k6/http';
import {sleep} from 'k6';

export let options = {
    insecureSkipTLSVerify: true,
    noConnectionsReuse: false,
    stages: [
        {duration: '5m', target: 300},
        {duration: '10m', target: 300},
        {duration: '5m', target: 0}
    ],
    thresholds: {
        http_req_duration: ['p(99)<200']
    }
}

const API_BASE_URL = "https://localhost:5001/api"

export default () => {
    http.batch([
        ['GET', `${API_BASE_URL}/Users`],
        ['GET', `${API_BASE_URL}/Users/paged?Page=1&PageSize=5&SortColumn=FirstName`]
    ])
    sleep(1)
}