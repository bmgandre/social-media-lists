export class ApiHeaders {
  static get defaultHeaders(): { [name: string]: string } {
    return {
      'Content-Type': 'application/json'
    };
  }
}
