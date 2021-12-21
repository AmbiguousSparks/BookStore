import http from 'k6/http';
import { sleep } from 'k6';

export let options = {
    insecureSkipTLSVerify: true,
    noConnectionReuse: false,
    vus: 10,
    duration: '30s'
};

export default () => {
    const random = Math.floor(Math.random() * 3500);
    const params = {
        headers: {
            'Content-Type': 'application/json',
        },
    };
    const payload = JSON.stringify({
        'firstName': "Daniel",
        'lastName': "Santos",
        'email': `teste${random}@gmail.com`,
        'password': "S0m3StrongP@assword",
        'confirmPassword': "S0m3StrongP@assword"
    });
    http.post('https://localhost:7194/api/Users', payload, params);
    sleep(2)
}