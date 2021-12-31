import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Subject } from 'rxjs';
import { AutoResponse } from '../models/autoresponse.model';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ResponsesService {

  $responses: Subject<AutoResponse[]>;

  constructor(private httpClient: HttpClient) {
    this.$responses = new Subject();
  }

  public getResponses(): void {
    this.httpClient.get(`${environment.API_URL}/auto-responses`).subscribe((res: any) => {
      let responses = res as any[];
      let results: AutoResponse[] = [];

      responses.forEach(response => {
          results.push(AutoResponse.Parse(response));
      });

      this.$responses.next(results);
    });
  }

  public addAutoResponse(response: AutoResponse): void {
    this.httpClient.post(`${environment.API_URL}/auto-responses`, response)
        .subscribe(() => this.getResponses());
  }

  public updateResponse(response: AutoResponse): void {
    this.httpClient.put(`${environment.API_URL}/auto-responses/${response.id}`, response)
        .subscribe(() => this.getResponses());
  }

  public deleteAutoResponse(id: string): void {
      this.httpClient.delete(`${environment.API_URL}/auto-responses/${id}`)
          .subscribe(() => this.getResponses());
  }

}
