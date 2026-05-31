
import type Dictionary from './Dictionary';

declare global {
  interface Window {
    __ENV__?: Record<string, string>
  }
}

export {}
