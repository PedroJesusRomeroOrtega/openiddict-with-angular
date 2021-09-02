import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { WeatherForecast } from './weather-forecast';

@Injectable({
  providedIn: 'root',
})
export class ForecastService {
  weatherForecast$ = this.http.get<WeatherForecast[]>(
    `${environment.baseUrlApi}WeatherForecast`
  );

  constructor(private http: HttpClient) {}
}
