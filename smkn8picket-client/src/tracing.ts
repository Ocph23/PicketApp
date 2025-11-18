// // src/tracing.js
// import { WebTracerProvider } from '@opentelemetry/sdk-trace-web';
// import { ConsoleSpanExporter, SimpleSpanProcessor } from '@opentelemetry/sdk-trace-base';
// import { registerInstrumentations } from '@opentelemetry/instrumentation';
// import { XMLHttpRequestInstrumentation } from '@opentelemetry/instrumentation-xml-http-request';
// import { FetchInstrumentation } from '@opentelemetry/instrumentation-fetch';
// import { W3CTraceContextPropagator, trace, context } from '@opentelemetry/api';

// // buat tracer
// const provider = new WebTracerProvider();
// provider.addSpanProcessor(new SimpleSpanProcessor(new ConsoleSpanExporter()));
// provider.register({
//   propagator: new W3CTraceContextPropagator(),
// });

// registerInstrumentations({
//   instrumentations: [
//     new XMLHttpRequestInstrumentation(),
//     new FetchInstrumentation(),
//   ],
// });

// export const tracer = trace.getTracer('vue-client');
