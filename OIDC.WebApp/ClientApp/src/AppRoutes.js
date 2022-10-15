import { Counter } from "./components/Counter";
import { FetchData } from "./components/FetchData";
import { Home } from "./components/Home";
import { ProtectedRoute } from "./components/ProtectedRoute";

const AppRoutes = [
  {
    index: true,
    element: <Home />
  },
  {
    path: '/counter',
    element: <ProtectedRoute> <Counter /> </ProtectedRoute>
  },
  {
    path: '/fetch-data',
    element: <ProtectedRoute> <FetchData /> </ProtectedRoute>
  }
];

export default AppRoutes;
