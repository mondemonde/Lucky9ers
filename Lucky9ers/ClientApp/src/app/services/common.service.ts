
import { throwError as observableThrowError, Observable, BehaviorSubject } from 'rxjs';

import { map, catchError } from 'rxjs/operators';
import { EventEmitter, Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';


@Injectable()
export class CommonService {
    constructor(private http: HttpClient) { }

    handleObservableHttpError(error: any): Observable<any> {
      console.log('server error:', error);  // debug
      if (error instanceof HttpErrorResponse) {
          return observableThrowError(error);
      }
      return observableThrowError(JSON.stringify(error) || 'backend server error');
  }



}
