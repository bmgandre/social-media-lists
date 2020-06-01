import { ApiCollection } from './api-collection';
import { environment } from '../../environments/environment';

export class ApiEndpoint {
  static apiPrefix = environment.apiUrl;
  static get posts() { return `${this.apiPrefix}/${ApiCollection.posts}`; }
  static get weather() { return `${this.apiPrefix}/${ApiCollection.weather}`; }
}
