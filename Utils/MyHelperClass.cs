﻿using Microsoft.AnalysisServices.AdomdClient;

namespace ApiCubeTB.Utils
{
    public class MyHelperClass
    {
        // Cadena de conexion del modelo multidimensional
        string connectionString = "Data Source=localhost\\SSAS;Catalog=TechBridgeOLAP;";

        // Metodo que crea una conexion al modelo multidimensional
        public AdomdConnection GetConnection()
        {
            AdomdConnection cnx = new AdomdConnection(connectionString);
            cnx.Open();
            return cnx;
        }


        // Cadena de conexion del modelo tabular
        string connectionStringTabular = "Provider=MSOLAP;Data Source=localhost\\SSASTabular;Catalog=TechBridgeTabular;";
        // Metodo que crea una conexion al modelo tabular
        public AdomdConnection GetConnectionTabular()
        {
            AdomdConnection cnx = new AdomdConnection(connectionStringTabular);
            cnx.Open();
            return cnx;
        }


        public List<Dictionary<string, object>> TransformToJSON(CellSet result)
        {
            var jsonData = new List<Dictionary<string, object>>();
            int cellIndex = 0; // Índice para rastrear las celdas correctamente

            // Iterar a través de las filas dinámicamente
            foreach (var rowPosition in result.Axes[1].Positions)  // Eje de filas (Dimensiones)
            {
                var dataPoint = new Dictionary<string, object>();

                // Agregar las dimensiones (desde el eje de filas)
                for (int i = 0; i < rowPosition.Members.Count; i++)
                {
                    var dimensionName = result.Axes[1].Set.Hierarchies[i].Name;
                    dataPoint[dimensionName] = rowPosition.Members[i].Caption; // Añadir nombre de la dimensión y valor
                }

                // Añadir las medidas correspondientes (desde el eje de columnas)
                for (int colIndex = 0; colIndex < result.Axes[0].Positions.Count; colIndex++)
                {
                    var measureName = result.Axes[0].Positions[colIndex].Members[0].Caption; // Captura el nombre de la medida
                    var cellValue = result.Cells[cellIndex].Value; // Captura el valor correcto de la celda

                    dataPoint[measureName] = cellValue;
                    cellIndex++; // Aumentar el índice de la celda
                }

                jsonData.Add(dataPoint);
            }

            return jsonData;
        }


    }
}
