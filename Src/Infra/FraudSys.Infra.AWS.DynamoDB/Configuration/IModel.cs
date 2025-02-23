namespace FraudSys.Infra.AWS.DynamoDB.Configuration;

public interface IModel<TModel, TEntity, TId>
{
    TModel EntityToModel(TEntity entity);
    TEntity ModelToEntity(TModel model);
    Dictionary<string, AttributeValue> ToAttributeMap(TModel model);
    TModel FromAttributeMap(Dictionary<string, AttributeValue> attributeMap);
    TId GetIdFromEntity(TEntity entity);
}