import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';

import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

import { ApiHeaders } from './api-headers';
import { CustomEncoder } from './custom-encoder';

@Injectable({
  providedIn: 'root',
})
export class ApiService {
  private _headers: HttpHeaders = new HttpHeaders(ApiHeaders.defaultHeaders);
  private _baseUrl: string;
  private _operation: string;

  constructor(
    private readonly _httpClient: HttpClient
  ) { }

  get baseUrl(): string {
    return this._baseUrl;
  }

  set baseUrl(url: string) {
    this._baseUrl = url;
  }

  get operation(): string {
    return this._operation;
  }

  set operation(operation: string) {
    this._operation = operation;
  }

  get headers(): HttpHeaders {
    return this._headers;
  }

  set headers(headers: HttpHeaders) {
    this._headers = headers;
  }

  appendHeader(name: string, value: string | string[]): void {
    this._headers = this._headers.append(name, value);
  }

  getQuery<T>(terms: any): Observable<T[]>;
  getQuery<T>(operation: string, terms: any): Observable<T[]>;
  getQuery<T>(operationOrTerms: string | any, terms?: any): Observable<T[]> {

    const queryParams: any = (typeof terms === 'undefined')
      ? operationOrTerms
      : terms;

    const url: string = (typeof terms === 'undefined')
      ? this._baseUrl
      : `${this._baseUrl}/${operationOrTerms}`;

    this.removeEmptyProperties(queryParams);

    const request = this._httpClient
      .get(url, {
        headers: this._headers,
        params: new HttpParams({
          fromString: this.convertAnyToString(queryParams),
          encoder: new CustomEncoder()
        })
      })
      .pipe(
        map((response: T[]) => {
          return response;
        })
      );

    return request;
  }

  private convertAnyToString(obj: any): string {
    const result = Object.entries(obj)
      .filter(([key, val]) => {
        return (obj[key] && typeof obj[key] !== 'object');
      })
      .map(([key, val]) => {
        return `${key}=${val}`;
      }).join('&');
    return result;
  }

  private removeEmptyProperties(obj: any) {
    Object.keys(obj).forEach(key => {
      if (obj[key] && typeof obj[key] === 'object') {
        this.removeEmptyProperties(obj[key]);
      } else if (obj[key] == null) {
        delete obj[key];
      }
    });
  }
}
