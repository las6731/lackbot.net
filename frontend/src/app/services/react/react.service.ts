import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';
import { environment } from 'src/environments/environment';
import { AutoReact } from '../../models/autoreact.model';

@Injectable({
    providedIn: 'root'
})
export class ReactService {

    $reacts: Subject<AutoReact[]>;

    constructor(private httpClient: HttpClient) {
        this.$reacts = new Subject();
    }

    public getReacts(): void {
        this.httpClient.get(`${environment.API_URL}/auto-react`).subscribe((reacts: any[]) => {
            let results: AutoReact[] = [];

            reacts.forEach(react => {
                results.push(AutoReact.Parse(react));
            });

            this.$reacts.next(results);
        });
    }

    public addAutoReact(react: AutoReact): void {
        this.httpClient.post(`${environment.API_URL}/auto-react`, react)
            .subscribe(() => this.getReacts());
    }

    public updateEmoji(id: string, emoji: string): void {
        const headers = new HttpHeaders().set('Content-Type', 'application/json; charset=utf-8');
        this.httpClient.put(`${environment.API_URL}/auto-react/${id}`, `"${emoji}"`, { headers: headers })
            .subscribe(() => this.getReacts());
    }

    public replaceReact(react: AutoReact): void {
        this.httpClient.post(`${environment.API_URL}/auto-react/${react.id}`, react)
            .subscribe(() => this.getReacts());
    }

    public deleteReact(id: string): void {
        this.httpClient.delete(`${environment.API_URL}/auto-react/${id}`)
            .subscribe(() => this.getReacts());
    }
}
