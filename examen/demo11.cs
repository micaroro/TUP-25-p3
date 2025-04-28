interface ILectura<T> {
    T Get(int id);
    T[] GetAll();
}

interface IEscritura<T> {
    void Create(T t);
    void Delete(T t);
    void Update(T t);

}
interface Repositorio<T> : ILectura<T>, IEscritura<T> {
}


void Informar(ILectura datos){

}

void Ajustar(IRepositorio datos){
    
}