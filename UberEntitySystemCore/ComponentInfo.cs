using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UberEntitySystemCoreTests.Tutorial_3;

namespace UberEntitySystemCore
{
    public static class ComponentInfo
    {
        public class ComponentTypeNotRegisteredException : Exception
        {
            public readonly Type type;
            public ComponentTypeNotRegisteredException(Type type) : base(type.ToString()+" is somehow not registered as a type in the ComponentInfo")
            {
                this.type = type;
            }
        }
        private static Dictionary<int, Type> _typeRegister = null;
        private static Dictionary<Type, int> _idRegister = null;

        public static KeyValuePair<int, Type>[] export
        {
            get
            {
                if (_typeRegister == null) reset(); //Make sure its setup before running on it
                return _typeRegister.ToArray();
            }
        }

        public static KeyValuePair<int, Type>[] import {
            set
            {
                if (_typeRegister == null) reset(); //Make sure its setup before running on it
                foreach (KeyValuePair<int, Type> pair in value)
                {
                    if (_typeRegister.ContainsKey(pair.Key) && _typeRegister[pair.Key] == pair.Value) //this one is already setup
                        continue; //Skip this one go to next

                    if (_typeRegister.ContainsKey(pair.Key))
                    {
                        //ID contains something that is not the desired type
                        Type oldType = _typeRegister[pair.Key]; //Cache old (Now Orphaned) type at index
                        if (_idRegister.ContainsKey(pair.Value))
                        {
                            //Desired Type already registered
                            int oldID = _idRegister[pair.Value]; //Cache old ID of type
                            _typeRegister.Remove(oldID); //Remove the old ID of type
                            _typeRegister.Remove(pair.Key); //Remove the type at desired ID
                            _typeRegister.Add(pair.Key, pair.Value); //set type
                            _idRegister.Remove(oldType); //{PERPUSLY LEAVE OUT THIS LINE FOR DEBUG EXAMPLE} Remove the old type as it is now orphaned and no longer has an index
                            _idRegister[pair.Value] = pair.Key; //set id
                            registerType(oldType);
                        }
                        else
                        {
                            //Desired Type is not already registered
                            if (typeof(IComponent).IsAssignableFrom(pair.Value)) //If the type is an IComponent
                                throw new ComponentTypeNotRegisteredException(pair.Value); //then throw because that should not be possible.

                            //else displace the type at that index for safety
                            _typeRegister.Remove(pair.Key);
                            _idRegister.Remove(oldType);
                            registerType(oldType);
                        }
                    }
                    else
                    {
                        if (_idRegister.ContainsKey(pair.Value))
                        {
                            //Desired Type already registered
                            int oldID = _idRegister[pair.Value]; //Cache old ID of type
                            _typeRegister.Remove(oldID); //Remove the old ID of type
                            _typeRegister.Add(pair.Key, pair.Value); //set type
                            _idRegister[pair.Value] = pair.Key; //set id
                        }
                        else
                        {
                            //Desired Type is not already registered
                            if (typeof(IComponent).IsAssignableFrom(pair.Value)) //If the type is an IComponent
                                throw new ComponentTypeNotRegisteredException(pair.Value); //then throw because that should not be possible.

                            //else ignore
                        }
                    }
                }
            }
        }

        public static int getID<T>() where T : IComponent
        {
            try
            {
                if (_typeRegister == null) reset(); //Make sure its setup before running on it
                return _idRegister[typeof(T)];
            }
            catch (Exception)
            {
                throw new ComponentTypeNotRegisteredException(typeof(T)); //Throw error because this should not be possible.
            }
        }

        public static int getID(Type type)
        {
            try
            {
                if (_typeRegister == null) reset(); //Make sure its setup before running on it
                return _idRegister[type];
            }
            catch (Exception)
            {
                if (typeof(IComponent).IsAssignableFrom(type)) //Implements the interface IComponent and should be registered.
                    throw new ComponentTypeNotRegisteredException(type); //Throw error because this should not be possible.
                throw new KeyNotFoundException("There is no id for type {" + type.ToString() + "} as it does not apear to be a component.");
            }
        }

        public static Type getType(int typeID)
        {
            try
            {
                if (_typeRegister == null) reset(); //Make sure its setup before running on it
                return _typeRegister[typeID];
            }
            catch (Exception)
            {
                throw new KeyNotFoundException("There is no type with the id {" + typeID + "}");
            }
        }

        private static int nextID = 0;

        private static int registerType(Type type)
        {
            _typeRegister.Add(nextID, type);
            _idRegister.Add(type, nextID);
            nextID++;
            return nextID - 1;
        }

        public static void reset()
        {
            nextID = 0; //Reset ids


            if (_typeRegister == null)
                _typeRegister = new Dictionary<int, Type>(); //Type Register Not setup
            else
                _typeRegister.Clear(); //is setup just clear it out

            if (_idRegister == null)
                _idRegister = new Dictionary<Type, int>(); //ID Register Not setup
            else
                _idRegister.Clear(); //is setup just clear it out



            Type icomp = typeof(IComponent);
            //Scan Everything for components
            //Get all active assemblies and loop each one.
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                //Get and loop through all Types in current assembly
                foreach (Type type in assembly.GetTypes())
                {
                    if (icomp.IsAssignableFrom(type) == false) //Not a component
                        continue; //Skip the rest of this loop and go to next type.

                    registerType(type);
                }
            }
        }
    }
}
