import React, { useState, useEffect } from 'react';
import { useFetch } from '../helpers/useFetch';

export const FetchData = () => {
  const [forecast, setForecast] = useState();
  const [loading, setLoading] = useState(true);

  const {status, data} = useFetch('weatherforecast');

  useEffect(() => {
    setForecast(data);
    setLoading(false);
  },[data]);

  const renderForecastsTable = (forecasts) => {
    return (
      <table className='table table-striped' aria-labelledby="tabelLabel">
        <thead>
          <tr>
            <th>Date</th>
            <th>Temp. (C)</th>
            <th>Temp. (F)</th>
            <th>Summary</th>
          </tr>
        </thead>
        <tbody>
          {forecasts.map(forecast =>
            <tr key={forecast.date}>
              <td>{forecast.date}</td>
              <td>{forecast.temperatureC}</td>
              <td>{forecast.temperatureF}</td>
              <td>{forecast.summary}</td>
            </tr>
          )}
        </tbody>
      </table>
    );
  }

  let contents = loading
      ? <p><em>Loading...</em></p>
      : renderForecastsTable(forecast);

  return (
    <div>
      <h1 id="tabelLabel" >Weather forecast</h1>
      <p>This component demonstrates fetching data from the server.</p>
      {contents}
    </div>
  );

}