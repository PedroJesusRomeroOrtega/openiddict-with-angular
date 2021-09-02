import { Component, OnInit } from '@angular/core';
import { ForecastService } from './forecast.service';

@Component({
  selector: 'app-optional-feature',
  templateUrl: './optional-feature.component.html',
  styleUrls: ['./optional-feature.component.scss'],
})
export class OptionalFeatureComponent {
  weatherForecast$ = this.forecastService.weatherForecast$;

  constructor(private forecastService: ForecastService) {}
}
