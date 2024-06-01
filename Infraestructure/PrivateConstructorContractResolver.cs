using Newtonsoft.Json.Serialization;
using System.Reflection;

public class PrivateConstructorContractResolver : DefaultContractResolver
{
    protected override JsonObjectContract CreateObjectContract(Type objectType)
    {
        var contract = base.CreateObjectContract(objectType);

        var constructor = objectType
            .GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
            .OrderByDescending(c => c.GetParameters().Length)
            .FirstOrDefault();

        if (constructor != null)
        {
            contract.OverrideCreator = (args) =>
            {
                return constructor.Invoke(args);
            };

            // Necesitamos mapear los nombres de las propiedades a los parámetros del constructor
            var parameters = constructor.GetParameters();
            contract.CreatorParameters.Clear();
            foreach (var parameter in parameters)
            {
                var property = contract.Properties.FirstOrDefault(p => p.PropertyName.Equals(parameter.Name, StringComparison.OrdinalIgnoreCase));
                if (property != null)
                {
                    contract.CreatorParameters.Add(property);
                }
            }
        }

        return contract;
    }
}