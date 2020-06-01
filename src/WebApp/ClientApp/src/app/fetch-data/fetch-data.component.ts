import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ActivatedRoute, Router } from '@angular/router';
import { ApiService } from '../api/api-service';
import { ApiCollection } from '../api/api-collection';
import { ApiEndpoint } from '../api/api-endpoint';
import { Observable } from 'rxjs';
import { switchMap } from 'rxjs/operators';

@Component({
  selector: 'app-fetch-data',
  templateUrl: './fetch-data.component.html'
})
export class FetchDataComponent {
  postSearchRequest: PostSearchRequest = new PostSearchRequest();
  postSearchResponse$: Observable<PostSearchResponse[]>;

  constructor(
    private activatedRoute: ActivatedRoute,
    private router: Router,
    private apiService: ApiService) {

    this.apiService.baseUrl = ApiEndpoint.posts;

    this.postSearchResponse$ = this.activatedRoute.paramMap
      .pipe(
        switchMap(params => {
          const network = params.get('network');
          const lists = params.get('lists');
          const content = params.get('content');
          const dateBegin = params.get('dateBegin');
          const dateEnd = params.get('dateEnd');
          const pageStart = params.get('pageStart');
          const pageSize = params.get('pageSize');

          const searchRequest: PostSearchRequest = {
            content: content,
            dateRange: {
              begin: dateBegin,
              end: dateEnd
            },
            lists: lists == undefined ? undefined : lists.split(','),
            network: network,
            page: {
              from: +pageStart,
              size: +pageSize
            }
          };

          return this.apiService.getQuery<PostSearchResponse>('search', searchRequest);
        })
      );
  }

  onSubmit(): void {
    this.router.navigate(['/fetch-data', this.postSearchRequest]);
  }

}

class DateRange {
  begin: string;
  end: string;
}

class Page {
  from: number;
  size: number;
}

class PostSearchRequest {
  network: string;
  lists: string[] = [];
  content: string;
  dateRange: DateRange = new DateRange();
  page: Page = new Page();
}

class PostSearchResponse {
  date: string;
  network: string;
  link: string;
  content: string;
  account: string;
  lists: string[];
}
