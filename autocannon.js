const autocannon = require('autocannon');
const { PassThrough } = require('stream');

function runTest(endpoint) {
    const buffer = [];
    const outputStream = new PassThrough();

    outputStream.on('data', (data) => buffer.push(data));
    outputStream.pipe(process.stdout);

    console.log(`Starting test on endpoint: ${endpoint}`);

    const instance = autocannon({
        url: endpoint,
        //method: 'POST',
        method: 'GET',
        headers: {
            'Content-Type': 'application/json',
        },
        //body: JSON.stringify({}),
        connections: 100,
        pipelining: 1,
        duration: 10,
    });

    autocannon.track(instance, { outputStream });

    instance.on('done', () => {
        console.log(`Test finished for endpoint: ${endpoint}`);
    });

    instance.on('error', (err) => {
        console.error(`Error during test: ${err.message}`);
    });
}

const endpoint1 = 'http://localhost:5191/person/pg';
const endpoint2 = 'http://localhost:5191/person/mg';

const endpointget1 = 'http://localhost:5191/person/pg';
const endpointget2 = 'http://localhost:5191/person/mg';

runTest(endpoint1);
