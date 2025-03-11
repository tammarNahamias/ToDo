import axios from 'axios';

const apiUrl = "http://localhost:5021";

export default {
  getTasks: async () => {
    const result = await axios.get(`${apiUrl}/getall`);
    return result.data;
  },

  addTask: async (name) => {
    console.log('addTask', name);
    const result = await axios.post(`${apiUrl}/add/${name}` ); // שינוי ל-POST
    return result.data;
  },

  setCompleted: async (id, isComplete) => {
    console.log('setCompleted', { id, isComplete });
    const result = await axios.put(`${apiUrl}/update/${id}`, { isComplete }); // שינוי ל-PUT
    return result.data;
  },

  deleteTask: async (id) => {
    const result = await axios.delete(`${apiUrl}/delete/${id}`);
    console.log('deleteTask');
    return result.data;
  }
};
